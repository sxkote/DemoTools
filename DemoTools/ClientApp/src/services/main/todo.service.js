import { __decorate } from "tslib";
import { injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import axios from "axios";
import { TodoList } from "../../classes/main/todo.class";
let TodoService = class TodoService {
    getTodoLists() {
        return axios.get(SYMBOLS.APIURL + 'todo')
            .then(response => {
            return response.data.map((i) => new TodoList(i));
        });
    }
    getTodoList(listID) {
        return axios.get(SYMBOLS.APIURL + 'todo/' + listID)
            .then(response => {
            return new TodoList(response.data);
        });
    }
    addTodoList(title) {
        throw new Error("Method not implemented.");
    }
    addTodoItem(listID, title) {
        throw new Error("Method not implemented.");
    }
    completeTodoItem(listID, itemID) {
        throw new Error("Method not implemented.");
    }
    deleteTodoItem(listID, itemID) {
        throw new Error("Method not implemented.");
    }
    modifyTodoItem(listID, itemID, title) {
        throw new Error("Method not implemented.");
    }
};
TodoService = __decorate([
    injectable()
], TodoService);
export { TodoService };
//# sourceMappingURL=todo.service.js.map