import { SharedService } from './shared/services/shared.service';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './modules/core/core.module';

import { AppComponent } from './shared/views/app/app.component';
import { AppInfoComponent } from './shared/views/app-info/app-info.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';
import { TestComponent } from './shared/components/test/test.component';
import { FormsModule } from '@angular/forms';
import {CommonModule} from '@angular/common';
import {RecordsModule} from './modules/records/records.module';
import { ToastsComponent } from './shared/components/toasts/toasts.component';

@NgModule({
  declarations: [
    AppComponent,
    AppInfoComponent,
    NavMenuComponent,
    TestComponent,
    ToastsComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    NgbModule,
    AppRoutingModule,
    CoreModule,
    RecordsModule
  ],
  providers: [
    SharedService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
