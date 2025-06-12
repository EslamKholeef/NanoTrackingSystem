import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserInfo } from 'src/app/core/models/auth.models';
import { AuthService } from 'src/app/core/services/auth.service';
import { MatToolbar } from '@angular/material/toolbar';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {

currentUser: UserInfo | null = null;
  selectedTab = 0;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      if (!user) {
        this.router.navigate(['/login']);
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  onTabChange(index: number): void {
    this.selectedTab = index;
  }
}