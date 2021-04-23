export interface ITodoItem {
  ID: string;
  Title: string;
  IsDone: boolean;
}

export interface ITodoList {
  ID: string;
  Title: string;
  Items: ITodoItem[];
}


export class TodoList implements ITodoList {
  ID = '';
  Title = '';
  Items: Array<TodoItem> = [];

  constructor(data?: any) {
    if (data !== undefined && data !== null) {
      this.ID = data.ID || '';
      this.Title = data.Title || '';
      if (!!data.Items) {
        this.Items = data.Items.map((i: any) => new TodoItem(i))
          .sort((a: TodoItem, b: TodoItem) => a.Title < b.Title ? -1 : 1);
      }
    }
  }
}

export class TodoItem implements ITodoItem {
  ID = '';
  Title = '';
  IsDone = false;

  constructor(data?: any) {
    if (data !== undefined && data !== null) {
      this.ID = data.ID || '';
      this.Title = data.Title || '';
      this.IsDone = data.IsDone === true;
    }
  }
}
