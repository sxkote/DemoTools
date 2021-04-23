import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {TodoListsComponent} from './views/todo-lists/todo-lists.component';
import {TodosService} from './services/todos.service';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {AppRoutingModule} from '../../app-routing.module';
import { TodoListComponent } from './views/todo-list/todo-list.component';


@NgModule({
  declarations: [
    TodoListsComponent,
    TodoListComponent,
  ],
  imports: [
    FormsModule,
    BrowserModule,
    NgbModule,
    HttpClientModule,
    RouterModule,
    AppRoutingModule,
  ],
  providers: [
    TodosService
  ],
})
export class RecordsModule {
}
