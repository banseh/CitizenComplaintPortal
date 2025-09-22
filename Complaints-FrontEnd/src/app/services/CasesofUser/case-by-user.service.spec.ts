import { TestBed } from '@angular/core/testing';

import { CaseByUserService } from './case-by-user.service'; 

describe('CaseByUserService', () => {
  let service: CaseByUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CaseByUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
