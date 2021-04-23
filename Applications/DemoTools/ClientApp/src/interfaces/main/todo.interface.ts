import { TodoList } from "../../classes/main/todo.class";

export interface ITodoService {
    getTodoLists(): Promise<Array<TodoList>>;
    getTodoList(listID: string): Promise<TodoList>;
    addTodoList(title: string): Promise<void>;
    modifyTodoList(listID: string, title: string): Promise<void>;
    deleteTodoList(listID: string): Promise<void>;


    toggleTodoListItem(listID: string, itemID: string): Promise<void>;
    addTodoListItem(listID: string, title: string): Promise<void>;
    deleteTodoListItem(listID: string, itemID: string): Promise<void>;
    modifyTodoListItem(listID: string, itemID: string, title: string): Promise<void>;
}