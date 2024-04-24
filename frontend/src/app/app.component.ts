import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { ToolbarComponent } from './core/toolbar/toolbar.component';
import { MenuComponent } from './core/menu/menu.component';
import { TelemetryService } from '@services/telemetry.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MatSidenavModule, ToolbarComponent, MenuComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  constructor(private telemetryService: TelemetryService) {}

  ngOnInit(): void {
    this.telemetryService.initialize();
  }
}
