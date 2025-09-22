import { TestBed } from '@angular/core/testing';

import { ContactServeiceService } from './contact-serveice.service';

describe('ContactServeiceService', () => {
  let service: ContactServeiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContactServeiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
