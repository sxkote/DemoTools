import { __decorate } from "tslib";
import { injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import axios from "axios";
import { TodoList } from "../../classes/main/todo.class";
let TodoService = class TodoService {
    getTodoLists() {
        return axios.get(SYMBOLS.APIURL + 'todo')
            .then(response => {
            return response.data.map((i) => new TodoList(i))
                .sort((a, b) => a.Title < b.Title ? -1 : 1);
        });
    }
    getTodoList(listID) {
        return axios.get(SYMBOLS.APIURL + 'todo/' + listID)
            .then(response => {
            return new TodoList(response.data);
        });
    }
    addTodoList(title) {
        return axios.post(SYMBOLS.APIURL + 'todo', {
            Title: title
        });
    }
    modifyTodoList(listID, title) {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID, {
            Title: title
        });
    }
    deleteTodoList(listID) {
        return axios.delete(SYMBOLS.APIURL + 'todo/' + listID);
    }
    toggleTodoListItem(listID, itemID) {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID + '/items/' + itemID + '/toggle');
    }
    addTodoListItem(listID, title) {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID + '/items', {
            Title: title
        });
    }
    modifyTodoListItem(listID, itemID, title) {
        return axios.post(SYMBOLS.APIURL + 'todo/' + listID + '/items/' + itemID, {
            Title: title
        });
    }
    deleteTodoListItem(listID, itemID) {
        return axios.delete(SYMBOLS.APIURL + 'todo/' + listID + '/items/' + itemID);
    }
};
TodoService = __decorate([
    injectable()
], TodoService);
export { TodoService };
//# sourceMappingURL=todo.service.js.map