import { TrackStatus } from './track-status.model';

export interface Track {
  id: string;
  title: string;
  artistId: string;
  artistName: string;
  isrc: string;
  releaseDate: string;
  genre: string;
  status: TrackStatus;
}
