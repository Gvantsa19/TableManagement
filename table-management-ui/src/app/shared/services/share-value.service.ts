import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ShareValueService {
  public message: any = [];
  public messageSourceDep = new BehaviorSubject(this.message);
  currentMessageDep = this.messageSourceDep.asObservable();

  constructor() {}

  changeMessageDep(dep: string) {
    this.messageSourceDep.next(dep);
  }
}
