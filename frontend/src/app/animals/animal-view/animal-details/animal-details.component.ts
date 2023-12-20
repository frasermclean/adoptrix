import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { Animal } from '@models/animal.model';
import { AgePipe } from '@pipes/age.pipe';

@Component({
  selector: 'app-animal-details',
  standalone: true,
  imports: [MatListModule, DatePipe, AgePipe],
  templateUrl: './animal-details.component.html',
  styleUrl: './animal-details.component.scss',
})
export class AnimalDetailsComponent {
  @Input({ required: true }) animal!: Animal;
}
