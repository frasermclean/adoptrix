import { Component, Input, OnInit, input } from '@angular/core';
import { sha256 } from 'js-sha256';

@Component({
  selector: 'app-avatar',
  standalone: true,
  templateUrl: './avatar.component.html',
  styleUrl: './avatar.component.scss',
})
export class AvatarComponent implements OnInit {
  @Input({ required: true }) email: string = null!;
  @Input() size: number = 48;
  @Input() borderWidth: number = 2;

  gravatarUrl: string = '';

  ngOnInit() {
    const hash = sha256(this.email.trim().toLowerCase());
    this.gravatarUrl = `https://www.gravatar.com/avatar/${hash}`;
  }
}
