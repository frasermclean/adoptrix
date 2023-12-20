import { Component, Input, ViewChild } from '@angular/core';
import { ImageInformation } from '@models/image-information.model';
import { Gallery, GalleryItem, GalleryModule } from 'ng-gallery';

@Component({
  selector: 'app-image-gallery',
  standalone: true,
  imports: [GalleryModule],
  templateUrl: './image-gallery.component.html',
  styleUrl: './image-gallery.component.scss',
})
export class ImageGalleryComponent {
  @Input({ required: true }) images!: ImageInformation[];

  readonly galleryItems: GalleryItem[] = [];

  constructor(private galleryService: Gallery) {}
}
