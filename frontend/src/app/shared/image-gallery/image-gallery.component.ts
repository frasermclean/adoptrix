import { Component, Input, OnInit } from '@angular/core';
import { ImageInformation } from '@models/image-information.model';
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
  @Input({ required: true }) images!: ImageInformation[];

  readonly galleryId = 'gallery';
  galleryItems: GalleryItem[] = [];

  constructor(private galleryService: Gallery) {}

  ngOnInit(): void {
    this.galleryItems = this.images.map(
      (image) => new ImageItem({ src: image.uri, thumb: image.uri, args: { description: image.description } })
    );
    const galleryRef = this.galleryService.ref(this.galleryId);
    galleryRef.load(this.galleryItems);
  }
}
