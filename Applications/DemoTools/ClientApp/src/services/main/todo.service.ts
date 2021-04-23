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
                return response.data.map((i: any) => new TodoList(i))
                    .sort((a: TodoList, b: TodoList) => a.Title < b.Title ? -1 : 1);
            });
    }
    getTodoList(listID: string): Promise<TodoList> {
        return axios.get(SYMBOLS.APIURL + 'todo/' + listID)
            .then(response => {
                return new TodoList(response.data);
            });
    }
    addTodoList(title: string): Promise<void> {
        return axios.post(SYMBOLS.APIURL + 'todo', {
            Title: title
        });
    }
    modifyTodoList(listID: string, title: string): Promise<void> {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID, {
            Title: title
        });
    }
    deleteTodoList(listID: string): Promise<void> {
        return axios.delete(SYMBOLS.APIURL + 'todo/' + listID);
    }


    toggleTodoListItem(listID: string, itemID: string): Promise<void> {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID + '/items/' + itemID + '/toggle');
    }
    addTodoListItem(listID: string, title: string): Promise<void> {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID + '/items', {
            Title: title
        });
    }
    modifyTodoListItem(listID: string, itemID: string, title: string): Promise<void> {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID + '/items/' + itemID, {
            Title: title
        });
    }
    deleteTodoListItem(listID: string, itemID: string): Promise<void> {
        return axios.delete(SYMBOLS.APIURL + 'todo/' + listID + '/items/' + itemID);
    }
}
