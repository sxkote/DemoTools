import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './modules/core/views/login/login.component';
import { TestComponent } from './shared/components/test/test.component';
import { AppInfoComponent } from './shared/views/app-info/app-info.component';
import {TodoListsComponent} from './modules/records/views/todo-lists/todo-lists.component';
import {TodoListComponent} from './modules/records/views/todo-list/todo-list.component';
import {RegistrationComponent} from './modules/core/views/registration/registration.component';
import {ProfileComponent} from './modules/core/views/profile/profile.component';

const routes: Routes = [
  { path: '', component: AppInfoComponent },
  { path: 'info', component: AppInfoComponent },
  { path: 'test', component: TestComponent },
  { path: 'todos', component: TodoListsComponent },
  { path: 'todos/:id', component: TodoListComponent },

  { path: 'login', component: LoginComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'registration', component: RegistrationComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
