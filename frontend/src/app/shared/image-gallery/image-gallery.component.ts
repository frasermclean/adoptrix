import { Component, Input, OnInit } from '@angular/core';
import { ImageResponse } from '@models/image.response';
import { ImageUrlService } from '@services/image-url.service';
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
  @Input({ required: true }) images!: ImageResponse[];

  readonly galleryId = 'gallery';
  galleryItems: GalleryItem[] = [];

  constructor(private galleryService: Gallery, private imageUrlService: ImageUrlService) {}

  ngOnInit(): void {
    this.galleryItems = this.images.map(
      (image) =>
        new ImageItem({
          src: this.imageUrlService.getFullSizeUrl(this.animalId, image),
          thumb: this.imageUrlService.getThumbnailUrl(this.animalId, image),
          args: { description: image.description },
        })
    );
    const galleryRef = this.galleryService.ref(this.galleryId);
    galleryRef.load(this.galleryItems);
  }
}
