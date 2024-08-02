import {Injectable} from '@angular/core';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  constructor() {
  }

  getInfoFromToken(storedToken: any): any {
    return jwtDecode<any>(storedToken);
  }
}
