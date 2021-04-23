import {Component, OnInit} from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import {TodosService} from '../../services/todos.service';
import {TodoItem, TodoList} from '../../classes/todo.class';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'dt-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss'],
})
export class TodoListComponent implements OnInit {

  listID = '';
  list: TodoList|null = null;
  modelModify = {
    Mode: '',
    Title: ''
  };

  constructor(private route: ActivatedRoute, private todosService: TodosService, private modalService: NgbModal) {
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.listID = params.id;
      this.reload();
    });
  }

  reload(): void {
    this.todosService.getTodoList(this.listID)
      .subscribe(item => this.list = item);
  }

  createTodoItem(content: any): void {
    this.modelModify.Mode = 'Create';
    this.modelModify.Title = '';

    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.todosService.addTodoListItem(this.listID, this.modelModify.Title)
          .subscribe(() => {
            this.reload();
          });
      }
    }, (reason) => {
    });
  }

  modifyTodoItem(item: TodoItem, content: any): void {
    this.modelModify.Mode = 'Modify';
    this.modelModify.Title = item.Title;

    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.todosService.modifyTodoListItem(this.listID, item.ID, this.modelModify.Title)
          .subscribe(() => {
            this.reload();
          });
      }
    }, (reason) => {
    });
  }

  deleteTodoItem(item: TodoItem, content: any): void {
    this.modelModify.Mode = 'Delete';
    this.modelModify.Title = item.Title;

    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.todosService.deleteTodoListItem(this.listID, item.ID)
          .subscribe(() => {
            this.reload();
          });
      }
    }, (reason) => {
    });
  }
}
