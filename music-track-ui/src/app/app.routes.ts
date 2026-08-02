import { Routes } from '@angular/router';
import { TrackListComponent } from './features/tracks/track-list/track-list.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'tracks' },
  { path: 'tracks', component: TrackListComponent }
];
