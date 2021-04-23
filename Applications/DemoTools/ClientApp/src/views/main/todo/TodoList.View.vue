<template>
    <div class="container">
        <Loader title="loading" :isLoading="isLoading">
            <div>
                <div class="row">
                    <div class="col-md-6">
                        <h2>Todo List: {{list.Title}}</h2>
                    </div>
                    <div class="col-md-6 d-sm-none d-md-block">
                        <router-link :to="{ name: 'TodoLists'}" class="link float-end mt-2">back</router-link>
                    </div>
                </div>

                <div>
                    <button class="btn btn-outline-success" @click="editItem(null)">new item</button>
                </div>
                <div class="mt-3">
                    <ul class="list-group">
                        <li class="list-group-item" v-for="i in list.Items">
                            <div class="d-inline-block pt-1">
                                <a class="link h5" v-bind:class="{done: i.IsDone }" @click="toggleItem(i)">{{i.Title}}</a>
                            </div>
                            <div class="btn-group btn-group-sm float-end">
                                <button class="btn btn-sm btn-outline-secondary" @click="editItem(i)">edit</button>
                                <button class="btn btn-sm btn-outline-danger" @click="removeItem(i)">remove</button>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </Loader>
    </div>

    <TitleModal :isShown="modalEdit" :model="model" @close="modifyItem($event)"></TitleModal>
    <ConfirmModal :isShown="modalDelete" :model="model" @close="deleteItem($event)"></ConfirmModal>

</template>


<script lang="ts">
    import { Vue, Options } from 'vue-class-component';
    import { Inject } from 'vue-property-decorator';
    import { Container } from "inversify";
    import SYMBOLS from '@/configs/symbols';
    import { ITodoService } from '@/interfaces/main/todo.interface';
    import { TodoList, TodoItem } from '@/classes/main/todo.class';
    import Loader from '@/components/Loader.vue';
    import TitleModal from '@/components/modals/Title.Modal.vue'
    import ConfirmModal from '@/components/modals/Confirm.Modal.vue'

    @Options({
        components: {
            Loader,
            TitleModal,
            ConfirmModal
        },
    })
    export default class TodoListView extends Vue {
        private todoService: ITodoService;
        private list: TodoList;
        private listID: any;
        private isLoading: boolean = false;
        private modalEdit: boolean = false;
        private modalDelete: boolean = false;
        private selectedItem: TodoItem | null = null;
        private model: any = { title: "" };

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        created(): void {
            this.todoService = this._container.get<ITodoService>(SYMBOLS.ITodoService);
            this.listID = this.$route.params.id;
            this.reload();
        }

        reload(): void {
            var $ctrl = this;
            this.isLoading = true;
            this.todoService.getTodoList(this.listID)
                .then(list => {
                    $ctrl.list = list;
                    $ctrl.isLoading = false;
                })
                .catch(err => {
                    $ctrl.isLoading = false;
                    console.log(err);
                });;
        }

        getSelectedItemTitle(): string {
            return (!this.selectedItem || !this.selectedItem.Title) ? "" : this.selectedItem.Title;
        }

        toggleItem(item: TodoItem): void {
            if (!item)
                return;

            var $self = this;
            this.todoService.toggleTodoListItem(this.listID, item.ID)
                .then(() => {
                    $self.reload();
                });
        }

        editItem(item: TodoItem): void {
            this.selectedItem = item;
            this.model = {
                caption: "Edit Todo Item",
                placeholder: "item title",
                title: this.getSelectedItemTitle()
            };
            this.modalEdit = true;
        }
        removeItem(item: TodoItem): void {
            if (!item || !item.ID)
                return;

            this.selectedItem = item;
            this.model = {
                caption: "Remove Todo Item",
                question: "Delete Todo Item: " + this.selectedItem.Title + "?"
            };
            this.modalDelete = true;
        }

        modifyItem(model: any): void {
            this.modalEdit = false;

            if (!model)
                return;

            var $self = this;
            if (!this.selectedItem) {
                this.todoService.addTodoListItem(this.listID, model.title)
                    .then(() => {
                        $self.reload();
                    });
            }
            else {
                this.todoService.modifyTodoListItem(this.listID, this.selectedItem.ID, model.title)
                    .then(() => {
                        $self.reload();
                    });
            }
        }
        deleteItem(model: any): void {
            this.modalDelete = false;

            if (model !== true || !this.selectedItem)
                return;

            var $self = this;
            this.todoService.deleteTodoListItem(this.listID, this.selectedItem.ID)
                .then(() => {
                    $self.reload();
                });
        }
    }
</script>

<style scoped>
    .done {
        color: #808080;
        text-decoration: line-through;
    }

    .list-group-item > .btn-group {
        visibility: hidden;
    }

    .list-group-item:hover > .btn-group {
        visibility: visible;
    }
</style>
