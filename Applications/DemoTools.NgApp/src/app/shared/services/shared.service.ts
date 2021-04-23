import {Injectable, TemplateRef} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';

@Injectable()
export class SharedService {
  toasts: any[] = [];

  constructor() {
  }

  // Push new Toasts to array with content and options
  showToastMessage(textOrTpl: string | TemplateRef<any>, options: any = {}): void {
    this.toasts.push({textOrTpl, ...options});
  }

  // Callback method to remove Toast DOM element from view
  removeToastMessage(toast: any): void {
    this.toasts = this.toasts.filter(t => t !== toast);
  }

  showToastSuccess(message: string): void {
    this.showToastMessage(message, {
      classname: 'bg-success text-light',
      delay: 6000,
      autohide: true
    });
  }

  showToastError(message: string): void {
    this.showToastMessage(message, {
      classname: 'bg-danger text-light',
      delay: 6000,
      autohide: true
    });
  }
}
