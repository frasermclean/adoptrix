import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalsService } from '@services/animals.service';
import { AnimalListItemComponent } from './animal-list-item/animal-list-item.component';

@Component({
  selector: 'app-animals',
  standalone: true,
  imports: [CommonModule, AnimalListItemComponent],
  templateUrl: './animals-list.component.html',
  styleUrl: './animals-list.component.scss',
})
export class AnimalsListComponent {
  animals$ = this.animalsService.searchAnimals();

  constructor(private animalsService: AnimalsService) {}
}
