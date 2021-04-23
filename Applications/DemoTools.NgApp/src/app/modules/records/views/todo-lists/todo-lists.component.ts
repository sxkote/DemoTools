import {Component, OnInit} from '@angular/core';
import {TodosService} from '../../services/todos.service';
import {TodoList} from '../../classes/todo.class';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'dt-todo-lists',
  templateUrl: './todo-lists.component.html',
  styleUrls: ['./todo-lists.component.scss'],
})
export class TodoListsComponent implements OnInit {

  items: TodoList[] = [];
  modelTodoListModify = {
    Mode: '',
    Title: ''
  };

  constructor(private todosService: TodosService, private modalService: NgbModal) {
  }

  ngOnInit(): void {
    this.reload();
  }

  reload(): void {
    this.todosService.getTodoLists()
      .subscribe(items => this.items = items);
  }

  createTodoList(content: any): void {
    this.modelTodoListModify.Mode = 'Create';
    this.modelTodoListModify.Title = '';

    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.todosService.addTodoList(this.modelTodoListModify.Title)
          .subscribe(() => {
            this.reload();
          });
      }
    }, (reason) => {
    });
  }

  modifyTodoList(item: TodoList, content: any): void {
    this.modelTodoListModify.Mode = 'Modify';
    this.modelTodoListModify.Title = item.Title;

    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.todosService.modifyTodoList(item.ID, this.modelTodoListModify.Title)
          .subscribe(() => {
            this.reload();
          });
      }
    }, (reason) => {
    });
  }

  deleteTodoList(item: TodoList, content: any): void {
    this.modelTodoListModify.Mode = 'Delete';
    this.modelTodoListModify.Title = item.Title;

    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.todosService.deleteTodoList(item.ID)
          .subscribe(() => {
            this.reload();
          });
      }
    }, (reason) => {
    });
  }
}
