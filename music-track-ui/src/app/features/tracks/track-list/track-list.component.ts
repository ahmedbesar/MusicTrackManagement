import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TrackService } from '../../../core/services/track.service';
import { Track, TrackStatus, TRACK_STATUSES } from '../../../core/models';

@Component({
  selector: 'app-track-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './track-list.component.html',
  styleUrl: './track-list.component.css'
})
export class TrackListComponent implements OnInit {
  private readonly trackService = inject(TrackService);

  readonly statuses = TRACK_STATUSES;
  readonly tracks = signal<Track[]>([]);
  readonly statusFilter = signal<TrackStatus | ''>('');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadTracks();
  }

  onStatusFilterChange(status: string): void {
    this.statusFilter.set(status as TrackStatus | '');
    this.loadTracks();
  }

  private loadTracks(): void {
    this.loading.set(true);
    this.error.set(null);

    this.trackService.getAll({ status: this.statusFilter() }).subscribe({
      next: (tracks) => {
        this.tracks.set(tracks);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load tracks. Please try again.');
        this.loading.set(false);
      }
    });
  }
}
