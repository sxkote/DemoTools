export class TodoList {
    constructor(data) {
        this.ID = "";
        this.Title = "";
        this.Items = [];
        if (data != undefined && data != null) {
            this.ID = data.ID || "";
            this.Title = data.Title || "";
            if (!!data.Items) {
                this.Items = data.Items.map((i) => new TodoItem(i))
                    .sort((a, b) => a.Title < b.Title ? -1 : 1);
            }
        }
    }
}
export class TodoItem {
    constructor(data) {
        this.ID = "";
        this.Title = "";
        this.IsDone = false;
        if (data != undefined && data != null) {
            this.ID = data.ID || "";
            this.Title = data.Title || "";
            this.IsDone = data.IsDone === true;
        }
    }
}
//# sourceMappingURL=todo.class.js.map