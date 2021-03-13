import { TodoList } from "../../classes/main/todo.class";

export interface ITodoService {
    getTodoLists(): Promise<Array<TodoList>>;
    getTodoList(listID: string): Promise<TodoList>;
    addTodoList(title: string): void;

    addTodoItem(listID: string, title: string): void;
    completeTodoItem(listID: string, itemID: string): void;
    deleteTodoItem(listID: string, itemID: string): void;
    modifyTodoItem(listID: string, itemID: string, title: string): void;
}