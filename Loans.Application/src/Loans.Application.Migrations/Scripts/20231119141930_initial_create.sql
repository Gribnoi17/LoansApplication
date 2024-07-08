create schema if not exists dcs_loans;

comment on schema dcs_loans is 'Схема для хранения клиентов и их кредитных заявок.';

create table if not exists dcs_loans.clients
(
    id serial primary key,
    last_name text not null,
    first_name text not null,
    middle_name text,
    birth_date date not null,
    salary decimal(11, 2)
 );

comment on table dcs_loans.clients is 'Таблица клиентов.';

comment on column dcs_loans.clients.id is 'Идентификатор клиента.';
comment on column dcs_loans.clients.first_name is 'Имя клиента.';
comment on column dcs_loans.clients.last_name is 'Фамилия клиента.';
comment on column dcs_loans.clients.middle_name is 'Отчество клиента.';
comment on column dcs_loans.clients.birth_date is 'Дата рождения клиента.';
comment on column dcs_loans.clients.salary is 'Зарплата клиента.';

create table if not exists dcs_loans.loan_applications
(
    id serial primary key,
    client_id int references dcs_loans.clients(id),
    amount decimal(11,2) not null,
    loan_term_month int not null,
    interest_rate decimal(4,2) not null,
    loan_date date not null,
    status int not null default '0',
    rejection_reason text
);

create index if not exists idx_loan_applications_client_id on dcs_loans.loan_applications(client_id);
create index if not exists idx_clients_first_name on dcs_loans.clients(first_name);
create index if not exists idx_clients_last_name on dcs_loans.clients(last_name);
create index if not exists idx_clients_middle_name on dcs_loans.clients(middle_name);
create index if not exists idx_clients_birth_date on dcs_loans.clients(birth_date);

comment on table dcs_loans.loan_applications is 'Таблица кредитных заявок.';

comment on column dcs_loans.loan_applications.id is 'Идентификатор кредитной заявки.';
comment on column dcs_loans.loan_applications.client_id is 'Идентификатор клиента, связанного с кредитной заявкой.';
comment on column dcs_loans.loan_applications.amount is 'Сумма кредита.';
comment on column dcs_loans.loan_applications.loan_term_month is 'Срок кредита в месяцах.';
comment on column dcs_loans.loan_applications.interest_rate is 'Процентная ставка по кредиту.';
comment on column dcs_loans.loan_applications.loan_date is 'Дата кредитной заявки.';
comment on column dcs_loans.loan_applications.status is 'Статус кредитной заявки.';
comment on column dcs_loans.loan_applications.rejection_reason is 'Причина отказа в случае, если статус заявки - отклонена.';

comment on index dcs_loans.idx_loan_applications_client_id is 'Индекс для поиска кредитных заявок по идентификатору клиента.';
comment on index dcs_loans.idx_clients_first_name is 'Индекс для поиска клиентов по имени.';
comment on index dcs_loans.idx_clients_last_name is 'Индекс для поиска клиентов по фамилии.';
comment on index dcs_loans.idx_clients_middle_name is 'Индекс для поиска клиентов по отчеству.';
comment on index dcs_loans.idx_clients_birth_date is 'Индекс для поиска клиентов по дате рождения.';
