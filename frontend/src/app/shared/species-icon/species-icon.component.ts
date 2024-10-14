import { Component, Input, OnInit } from '@angular/core';
import { FontAwesomeModule, SizeProp } from '@fortawesome/angular-fontawesome';
import { IconDefinition, faCat, faDog, faDove, faQuestion } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-species-icon',
  standalone: true,
  imports: [FontAwesomeModule],
  templateUrl: './species-icon.component.html',
  styleUrl: './species-icon.component.scss',
})
export class SpeciesIconComponent implements OnInit {
  @Input({ required: true }) speciesName!: string;
  @Input() size: SizeProp = '1x';
  icon: IconDefinition = faQuestion;

  ngOnInit(): void {
    switch (this.speciesName) {
      case 'Dog':
        this.icon = faDog;
        break;
      case 'Cat':
        this.icon = faCat;
        break;
      case 'Bird':
        this.icon = faDove;
        break;
      default:
        this.icon = faQuestion;
        break;
    }
  }
}
