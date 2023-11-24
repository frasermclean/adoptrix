import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalsService } from '../../services/animals.service';
import { Observable } from 'rxjs';
import { Animal } from '../../models/api-responses';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-animal-view',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule],
  templateUrl: './animal-view.component.html',
  styleUrl: './animal-view.component.scss',
})
export class AnimalViewComponent implements OnInit {
  @Input({ required: true }) animalId!: string;
  animal$!: Observable<Animal>;

  constructor(private animalsService: AnimalsService) {}

  ngOnInit(): void {
    this.animal$ = this.animalsService.getAnimal(this.animalId);
  }
}
