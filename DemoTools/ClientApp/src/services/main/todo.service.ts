import { injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import axios from "axios";
import { ITodoService } from "../../interfaces/main/todo.interface";
import { TodoList, TodoItem } from "../../classes/main/todo.class";

@injectable()
export class TodoService implements ITodoService {
    getTodoLists(): Promise<TodoList[]> {
        return axios.get(SYMBOLS.APIURL + 'todo')
            .then(response => {
                return response.data.map((i: any) => new TodoList(i));
            });
    }
    getTodoList(listID: string): Promise<TodoList> {
        return axios.get(SYMBOLS.APIURL + 'todo/' + listID)
            .then(response => {
                return new TodoList(response.data);
            });
    }
    addTodoList(title: string): void {
        throw new Error("Method not implemented.");
    }
    addTodoItem(listID: string, title: string): void {
        throw new Error("Method not implemented.");
    }
    completeTodoItem(listID: string, itemID: string): void {
        throw new Error("Method not implemented.");
    }
    deleteTodoItem(listID: string, itemID: string): void {
        throw new Error("Method not implemented.");
    }
    modifyTodoItem(listID: string, itemID: string, title: string): void {
        throw new Error("Method not implemented.");
    }
}
