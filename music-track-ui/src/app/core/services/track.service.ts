import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Track, TrackDetail, TrackStatus } from '../models';

export interface TrackFilters {
  artistId?: string;
  genre?: string;
  status?: TrackStatus | '';
}

@Injectable({ providedIn: 'root' })
export class TrackService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tracks`;

  getAll(filters: TrackFilters = {}): Observable<Track[]> {
    let params = new HttpParams();

    if (filters.artistId) params = params.set('artistId', filters.artistId);
    if (filters.genre) params = params.set('genre', filters.genre);
    if (filters.status) params = params.set('status', filters.status);

    return this.http.get<Track[]>(this.baseUrl, { params });
  }

  getById(id: string): Observable<TrackDetail> {
    return this.http.get<TrackDetail>(`${this.baseUrl}/${id}`);
  }
}
