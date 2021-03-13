<template>
    <div class="row" v-for="item in items">
        <div class="col">
            <a class="" @click="navigateToList(item.ID)">{{item.Title}}</a> <!--:href="'/Todo/' + item.ID"-->
        </div>
    </div>
</template>


<script lang="ts">
    import { Vue } from 'vue-class-component';
    import { Inject } from 'vue-property-decorator';
    import { Container } from "inversify";
    import SYMBOLS from '@/configs/symbols';
    import { ITodoService } from '@/interfaces/main/todo.interface';
    import { TodoList } from '@/classes/main/todo.class';
    import router from '@/configs/router.config';


    export default class TodoListsView extends Vue {
        private todoService: ITodoService;
        private items: Array<TodoList> = [];

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        login: string = "";
        password: string = "";

        created(): void {
            this.todoService = this._container.get<ITodoService>(SYMBOLS.ITodoService);
            this.reload();
        }

        reload(): void {
            var $ctrl = this;
            this.todoService.getTodoLists()
                .then(items => $ctrl.items = items);
        }

        navigateToList(listID: string): void {
            router.push('/todo/' + listID);
        }
    }
</script>

<style scoped>
</style>