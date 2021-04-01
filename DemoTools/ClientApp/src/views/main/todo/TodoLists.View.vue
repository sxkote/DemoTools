<template>
    <div class="container">
        <h2>Todo Lists</h2>
        <div>
            <button class="btn btn-outline-success" @click="editList(null)">new list</button>
        </div>
        <div class="mt-3">
            <ul class="list-group">
                <li class="list-group-item" v-for="item in items">
                    <div class="d-inline-block pt-1">
                        <router-link :to="{ name: 'TodoList', params: { id: item.ID }}" class="link h5">{{item.Title}}</router-link>
                    </div>
                    <div class="btn-group btn-group-sm float-end">
                        <button class="btn btn-sm btn-outline-secondary" @click="editList(item)">edit</button>
                        <button class="btn btn-sm btn-outline-danger" @click="removeList(item)">remove</button>
                    </div>
                </li>
            </ul>
        </div>

        <TitleModal :isShown="modalEdit" :model="model" @close="modifyList($event)"></TitleModal>
        <ConfirmModal :isShown="modalDelete" :model="model" @close="deleteList($event)"></ConfirmModal>

    </div>
</template>


<script lang="ts">
    import { Vue, Options } from 'vue-class-component';
    import { Inject } from 'vue-property-decorator';
    import { Container } from "inversify";
    import SYMBOLS from '@/configs/symbols';
    import { ITodoService } from '@/interfaces/main/todo.interface';
    import { TodoList } from '@/classes/main/todo.class';
    import TitleModal from '@/components/modals/Title.Modal.vue'
    import ConfirmModal from '@/components/modals/Confirm.Modal.vue'

    @Options({
        components: {
            TitleModal,
            ConfirmModal
        },
    })
    export default class TodoListsView extends Vue {
        private todoService: ITodoService;
        private items: Array<TodoList> = [];
        private modalEdit: boolean = false;
        private modalDelete: boolean = false;
        private selectedItem: TodoList | null = null;
        private model: any = { title: "" };

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        created(): void {
            this.todoService = this._container.get<ITodoService>(SYMBOLS.ITodoService);
            this.reload();
        }

        reload(): void {
            var $ctrl = this;
            this.todoService.getTodoLists()
                .then(items => $ctrl.items = items);
        }

        getSelectedItemTitle(): string {
            return (!this.selectedItem || !this.selectedItem.Title) ? "" : this.selectedItem.Title;
        }

        editList(list: TodoList): void {
            this.selectedItem = list;
            this.model = {
                caption: "Edit Toto List",
                placeholder: "todo list title",
                title: this.getSelectedItemTitle()
            };
            this.modalEdit = true;
        }
        removeList(list: TodoList): void {
            if (!list || !list.ID)
                return;

            this.selectedItem = list;
            this.model = {
                caption: "Delete Todo List",
                question: "Delete Todo list: " + this.selectedItem.Title + "?"
            };
            this.modalDelete = true;
        }

        modifyList(model: any): void {
            this.modalEdit = false;

            if (!model)
                return;

            var $self = this;
            if (!this.selectedItem) {
                this.todoService.addTodoList(model.title)
                    .then(() => {
                        $self.reload();
                    });
            }
            else {
                this.todoService.modifyTodoList(this.selectedItem.ID, model.title)
                    .then(() => {
                        $self.reload();
                    });
            }
        }
        deleteList(event: any): void {
            this.modalDelete = false;

            if (event !== true || !this.selectedItem)
                return;

            var $self = this;
            this.todoService.deleteTodoList(this.selectedItem.ID)
                .then(() => {
                    $self.reload();
                });
        }
    }
</script>


<style scoped>
    .list-group-item > .btn-group {
        visibility: hidden;
    }

    .list-group-item:hover > .btn-group {
        visibility: visible;
    }
</style>