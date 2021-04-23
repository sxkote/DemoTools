import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {SharedService} from '../../../shared/services/shared.service';
import {TodoList} from '../classes/todo.class';
import {Observable, throwError} from 'rxjs';
import {catchError, map, tap} from 'rxjs/operators';

@Injectable()
export class TodosService {
  constructor(private http: HttpClient, private sharedService: SharedService) {
  }

  private get APIURL(): string {
    return this.sharedService.API_RECORDS_URL;
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
