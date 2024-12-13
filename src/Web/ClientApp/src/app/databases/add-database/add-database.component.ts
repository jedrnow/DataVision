import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/common/toast/toast.service';
import { CreateDatabaseCommand, DatabaseProvider, DatabasesClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-database',
  templateUrl: './add-database.component.html'
})
export class AddDatabaseComponent implements OnInit {
  addDatabaseForm: FormGroup;
  connectionTested: boolean = false;
  isSubmitting: boolean = false;
  providerEnum = DatabaseProvider;
  availableProviders = Object.values(DatabaseProvider);

  constructor(
    private fb: FormBuilder,
    private client: DatabasesClient,
    private router: Router,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.addDatabaseForm = this.fb.group({
      name: ['', Validators.required],
      connectionString: ['', Validators.required],
      provider: [null, Validators.required],
    });
  }

  testConnection(): void {
    const connectionString = this.addDatabaseForm.get('connectionString')?.value;
    const databaseProvider = this.addDatabaseForm.get('provider')?.value;

    this.client.testConnection(databaseProvider, connectionString).subscribe({
        next: (result) => {
          if (result) {
            this.connectionTested = true;
            this.toastService.showSuccess('Connection successful!');
          } else {
            this.connectionTested = false;
            this.toastService.showError('Connection failed. Please check your credentials.');
          }
        },
        error: () => {
          this.connectionTested = false;
          this.toastService.showError('Connection failed. Please check your credentials.');
        },
      });
  }

  createDatabase(): void {
    if (this.addDatabaseForm.invalid || !this.connectionTested) {
      return;
    }

    const { name, connectionString, provider } = this.addDatabaseForm.value;

    const command = new CreateDatabaseCommand({name: name, connectionString: connectionString, databaseProvider: provider});


    this.isSubmitting = true;
    this.client.createDatabase(command).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/databases']);
          this.toastService.showSuccess('Database created successfully!');
        },
        error: (err) => {
          this.isSubmitting = false;
          this.toastService.showError('Error creating database: ' + err.message);
        },
      });
    }

  get isSubmitDisabled(): boolean {
    return this.isSubmitting || !this.connectionTested;
  }
}
