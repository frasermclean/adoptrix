import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [
    CommonModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    RouterModule,
  ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss',
})
export class MenuComponent {
  @Output() menuItemClicked = new EventEmitter<string>();

  menuItems = [
    { text: 'Home', icon: 'home', route: '/' },
    { text: 'Animals for adoption', icon: 'pets', route: '/animals' },
    { text: 'Admin portal', icon: 'admin_panel_settings', route: '/admin' },
  ];

  onClick(route: string) {
    this.menuItemClicked.emit(route);
  }
}
