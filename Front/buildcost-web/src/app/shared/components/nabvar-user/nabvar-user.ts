import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-navbar-user',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './nabvar-user.html',
  styleUrl: './nabvar-user.css',
})
export class NavbarUserComponent {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {}

  logout(): void {
    this.authService.logout();

    this.router.navigateByUrl('/login', {
      replaceUrl: true,
    });
  }
}
