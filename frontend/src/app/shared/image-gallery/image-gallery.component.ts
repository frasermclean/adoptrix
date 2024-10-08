import { Component, Input, OnInit } from '@angular/core';
import { AnimalImage } from '@models/animal.models';
import { Gallery, GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { LightboxModule } from 'ng-gallery/lightbox';

@Component({
  selector: 'app-image-gallery',
  standalone: true,
  imports: [GalleryModule, LightboxModule],
  templateUrl: './image-gallery.component.html',
  styleUrl: './image-gallery.component.scss',
})
export class ImageGalleryComponent implements OnInit {
  @Input({ required: true }) animalId!: string;
  @Input({ required: true }) images!: AnimalImage[];

  readonly galleryId = 'gallery';
  galleryItems: GalleryItem[] = [];

  constructor(private galleryService: Gallery) {}

  ngOnInit(): void {
    this.galleryItems = this.images.map(
      (image) =>
        new ImageItem({
          src: image.fullSizeUrl ?? '',
          thumb: image.thumbnailUrl ?? '',
          args: { description: image.description },
        })
    );
    const galleryRef = this.galleryService.ref(this.galleryId);
    galleryRef.load(this.galleryItems);
  }
}
