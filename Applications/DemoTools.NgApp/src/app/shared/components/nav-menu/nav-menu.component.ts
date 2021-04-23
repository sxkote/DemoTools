import {IToken} from 'src/app/interfaces/token.interface';
import {AuthenticationService} from 'src/app/modules/core/services/authentication.service';
import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'dt-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  constructor(private authenticationService: AuthenticationService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  isAuthenticated(): boolean {
    const token = this.authenticationService.Token;
    return !!token && token.isValid();
  }

  get Token(): IToken | null {
    return this.authenticationService.Token;
  }

  logout(): void {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

}
