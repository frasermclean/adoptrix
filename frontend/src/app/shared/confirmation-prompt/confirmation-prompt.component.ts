import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

export interface ConfirmationPromptData {
  title: string;
  message: string;
  confirmText: string;
  cancelText: string;
}

@Component({
  selector: 'app-confirmation-prompt',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  templateUrl: './confirmation-prompt.component.html',
  styleUrl: './confirmation-prompt.component.scss',
})
export class ConfirmationPromptComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: ConfirmationPromptData) {}
}
