import { TestBed } from '@angular/core/testing';

import { DeleteComplainService } from './delete-complain.service';

describe('DeleteComplainService', () => {
  let service: DeleteComplainService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteComplainService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
