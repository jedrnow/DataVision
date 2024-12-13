import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from 'src/app/common/toast/toast.service';
import { DatabaseProvider, DatabasesClient, UpdateDatabaseCommand } from 'src/app/web-api-client';

@Component({
  selector: 'app-edit-database',
  templateUrl: './edit-database.component.html'
})
export class EditDatabaseComponent implements OnInit {
  editDatabaseForm: FormGroup;
  connectionTested: boolean = false;
  isSubmitting: boolean = false;
  databaseId: string | null = null;
  providerEnum = DatabaseProvider;
  availableProviders = Object.values(DatabaseProvider);

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private client: DatabasesClient,
    private router: Router,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.databaseId = this.route.snapshot.paramMap.get('databaseId');

    if (this.databaseId) {
      this.client.getDatabaseDetails(+this.databaseId).subscribe(data => {
        this.editDatabaseForm = this.fb.group({
          name: [data.name, Validators.required],
          connectionString: [data.connectionString, Validators.required],
          provider: [data.provider, Validators.required],
        });
      });
    }

    this.editDatabaseForm = this.fb.group({
      name: ['', Validators.required],
      connectionString: ['', Validators.required],
      provider: [null, Validators.required],
    });
  }

  testConnection(): void {
    const connectionString = this.editDatabaseForm.get('connectionString')?.value;
    const databaseProvider = this.editDatabaseForm.get('provider')?.value;

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

  updateDatabase(): void {
    if (this.editDatabaseForm.invalid || !this.connectionTested) {
      return;
    }

    const { name, connectionString, provider } = this.editDatabaseForm.value;

    const command = new UpdateDatabaseCommand({id:+this.databaseId, name:name, connectionString:connectionString, databaseProvider:provider});

    this.isSubmitting = true;
    this.client.updateDatabase(+this.databaseId, command).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/databases']);
        this.toastService.showSuccess('Database updated successfully!');
      },
      error: (err) => {
        this.isSubmitting = false;
        this.toastService.showError('Error updating database: ' + err.message);
      },
    });
  }

  get isSubmitDisabled(): boolean {
    return this.isSubmitting || !this.connectionTested;
  }
}
