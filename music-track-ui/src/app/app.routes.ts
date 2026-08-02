import { Routes } from '@angular/router';
import { TrackListComponent } from './features/tracks/track-list/track-list.component';
import { TrackDetailComponent } from './features/tracks/track-detail/track-detail.component';
import { LoginComponent } from './features/auth/login/login.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'tracks' },
  { path: 'tracks', component: TrackListComponent },
  { path: 'tracks/:id', component: TrackDetailComponent },
  { path: 'login', component: LoginComponent }
];
