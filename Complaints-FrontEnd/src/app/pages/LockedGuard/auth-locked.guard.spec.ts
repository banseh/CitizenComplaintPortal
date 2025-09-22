import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { authLockedGuard } from './auth-locked.guard';

describe('authLockedGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => authLockedGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
