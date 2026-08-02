import { DatePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TrackService } from '../../../core/services/track.service';
import { TrackDetail } from '../../../core/models';

@Component({
  selector: 'app-track-detail',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './track-detail.component.html',
  styleUrl: './track-detail.component.css'
})
export class TrackDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly trackService = inject(TrackService);

  readonly track = signal<TrackDetail | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.error.set('No track id was provided.');
      this.loading.set(false);
      return;
    }

    this.trackService.getById(id).subscribe({
      next: (track) => {
        this.track.set(track);
        this.loading.set(false);
      },
      error: (response) => {
        this.error.set(
          response?.status === 404 ? 'Track not found.' : 'Failed to load track details.'
        );
        this.loading.set(false);
      }
    });
  }
}
