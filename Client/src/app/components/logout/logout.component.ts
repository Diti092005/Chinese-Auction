import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.scss'
})
export class LogoutComponent implements OnInit {
  private router = inject(Router);
  ngOnInit() {
    this.logout();
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
