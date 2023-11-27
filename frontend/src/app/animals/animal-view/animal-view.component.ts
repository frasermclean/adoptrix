import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalsService } from '@services/animals.service';
import { Observable } from 'rxjs';

import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { AgePipe } from '@pipes/age.pipe';
import { Animal } from '@models/animal.model';

@Component({
  selector: 'app-animal-view',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule, AgePipe],
  templateUrl: './animal-view.component.html',
  styleUrl: './animal-view.component.scss',
})
export class AnimalViewComponent implements OnInit {
  @Input({ required: true }) animalId!: string;
  animal$: Observable<Animal>;

  constructor(
    private animalsService: AnimalsService,
    private route: ActivatedRoute
  ) {
    const animalId = this.route.snapshot.params['animalId'];
    this.animal$ = this.animalsService.getAnimal(animalId).pipe();
  }

  ngOnInit(): void {
    this.animal$ = this.animalsService.getAnimal(this.animalId);
  }
}
