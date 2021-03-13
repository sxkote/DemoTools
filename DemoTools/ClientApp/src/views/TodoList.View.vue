<template>
    <div v-if="isLoaded">
        <h1>{{item.Title}}</h1>
        <div class="row" v-for="i in item.Items">
            <div class="col">
                <a class="" href="" @click="completeItem(i.ID)">{{i.Title}}</a>
            </div>
        </div>
    </div>
    <button class="btn btn-default" @click="toggleLoaded()">Toggle</button>
</template>


<script lang="ts">
    import { Vue } from 'vue-class-component';
    import { Inject } from 'vue-property-decorator';
    import { Container } from "inversify";
    import SYMBOLS from '@/configs/symbols';
    import { ITodoService } from '@/interfaces/main/todo.interface';
    import { TodoList } from '@/classes/main/todo.class';


    export default class TodoListView extends Vue {
        private todoService: ITodoService;
        private item: TodoList;
        private listID: any;
        private isLoaded: boolean = false;

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        created(): void {
            this.todoService = this._container.get<ITodoService>(SYMBOLS.ITodoService);
            this.listID = this.$route.params.id;
            this.reload();
        }

        reload(): void {
            var $ctrl = this;
            this.todoService.getTodoList(this.listID)
                .then(item => {
                    $ctrl.item = item;
                    $ctrl.isLoaded = true;
                });
        }

        completeItem(itemID: string): void {
            console.log(itemID + ' - completed!');
        }

        toggleLoaded(): void {
            this.isLoaded = !this.isLoaded;
        }
    }
</script>

<style scoped>
</style>