import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {TodoList} from '../classes/todo.class';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {environment} from 'src/environments/environment';

@Injectable()
export class TodosService {
  constructor(private http: HttpClient) {
  }

  private get APIURL(): string {
    return environment.recordsApiUrl;
  }

  getTodoLists(): Observable<TodoList[]> {
    return this.http.get<TodoList[]>(this.APIURL + 'todo')
      .pipe(
        map(result => result.sort((a, b) => a.Title < b.Title ? -1 : 1))
      );
  }

  getTodoList(listID: string): Observable<TodoList> {
    return this.http.get<TodoList>(this.APIURL + 'todo/' + listID);
  }

  addTodoList(title: string): Observable<never> {
    return this.http.post<never>(this.APIURL + 'todo', {
      Title: title
    });
  }

  modifyTodoList(listID: string, title: string): Observable<any> {
    return this.http.post(this.APIURL + 'todo/' + listID, {
      Title: title
    });
  }

  deleteTodoList(listID: string): Observable<any> {
    return this.http.delete(this.APIURL + 'todo/' + listID);
  }


  toggleTodoListItem(listID: string, itemID: string): Observable<any> {
    return this.http.post(this.APIURL + 'todo/' + listID + '/items/' + itemID + '/toggle', null);
  }

  addTodoListItem(listID: string, title: string): Observable<any> {
    return this.http.post(this.APIURL + 'todo/' + listID + '/items', {
      Title: title
    });
  }

  modifyTodoListItem(listID: string, itemID: string, title: string): Observable<any> {
    return this.http.post(this.APIURL + 'todo/' + listID + '/items/' + itemID, {
      Title: title
    });
  }

  deleteTodoListItem(listID: string, itemID: string): Observable<any> {
    return this.http.delete(this.APIURL + 'todo/' + listID + '/items/' + itemID);
  }


}
