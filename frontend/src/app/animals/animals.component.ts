import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalsService } from '../services/animals.service';

@Component({
  selector: 'app-animals',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './animals.component.html',
  styleUrl: './animals.component.scss',
})
export class AnimalsComponent {
  animals$ = this.animalsService.searchAnimals();

  constructor(private animalsService: AnimalsService) {}
}
