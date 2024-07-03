import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { UserService } from '../../../../shared/services/user.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TableService {
  baseUrl = environment.apiUrl;
  currentUserId = this.user.token();

  constructor(private http: HttpClient, private user: UserService) {}

  getAllTable() {
    const token = this.user.token();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.get(this.baseUrl + 'Table', { headers });
  }

  getById(tableName: string, pageNumber, pageSize): Observable<any> {
    const token = this.user.token();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    let params = new HttpParams()
      // .set('id', id.toString())
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      // .set('sortColumn', sortColumn)
      // .set('sortOrder', sortOrder)
      // .set('columnNameFilter', columnNameFilter)
      .set('tableName', tableName);
    return this.http.get<any>(this.baseUrl + `Table/getByName`, {
      headers,
      params,
    });
  }

  createTable(data) {
    const token = this.user.token();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.post(this.baseUrl + 'Table/create', data, { headers });
  }

  updateTable(tableName, data) {
    const token = this.user.token();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    let params = new HttpParams().set('tableName', tableName);

    return this.http.post(this.baseUrl + 'Table/insert', data, {
      headers,
      params,
    });
  }
}
