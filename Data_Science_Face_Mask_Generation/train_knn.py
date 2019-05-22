import numpy as np
from sklearn.preprocessing import StandardScaler
from sklearn.neighbors import KNeighborsClassifier
import dask.dataframe as dd
from sklearn.externals import joblib
import time


# Function that creates a numpy dataframe (Array) from our CSV file
def create_dataframe(csv_file):
    column_names = ["rbgb_" + str(number) for number in range(1, 148)]  # Generate the column names for RBB values
    column_names.append("classifier")  # Append the column name for the classifier
    df_start_time = time.time()
    print("Creating the matrix of RGB values...")
    dataset = dd.read_csv(csv_file, names=column_names, dtype=np.float64)  # Create the dataframe
    rgb_values = dataset.iloc[:, :-1].values  # Saves all columns except the last one into the rgb_values variable
    rgb_values = rgb_values.compute()
    rgb_classifier = dataset.iloc[:, 147].values  # Saves the last column (classifier) into the rgb_classifier variable
    rgb_classifier = rgb_classifier.compute()
    df_end_time = time.time()
    print("-> " + (str(round((df_end_time - df_start_time), 2)) + " seconds to create the matrix dataframe <-\n"))
    return rgb_values, rgb_classifier


# Function to normalize our data to prepare it for KNN
def normalize_rgb_values(rgb_values):
    scaler_start_time = time.time()
    print("Normalizing the data with a scaler...")
    scaler = StandardScaler()
    scaler.fit(rgb_values)  # Uses the StandardScaler function to normalize our data
    scaler_end_time = time.time()
    print("-> " + (str(round((scaler_end_time - scaler_start_time), 2)) + " seconds to normalize the data <-\n"))
    return rgb_values


def train_knn_algorithm(rgb_values, rgb_classifier, k_value, num_cpus):
    import_knn_start = time.time()
    print("Importing KNN algorithm and fitting data")
    classifier = KNeighborsClassifier(n_neighbors=k_value, n_jobs=num_cpus)  # Initialize k-NN with a value of 5 and use all CPUs
    classifier.fit(rgb_values, rgb_classifier)  # Fit our data to the k-nn algorithm
    import_knn_end = time.time()
    print("-> " + (str(round((import_knn_end - import_knn_start), 2)) + " seconds to train the data with k-NN <-\n"))
    return classifier


def create_knn_classifier(csv_file, knn_trained_classifier, k_value, num_cpus):
    print("Starting the k-NN training process")
    rgb_values, rgb_classifier = create_dataframe(csv_file)  # Create our dataframe from CSV
    normalized_rgb_values = normalize_rgb_values(rgb_values)  # Normalize our data
    classifier = train_knn_algorithm(normalized_rgb_values, rgb_classifier, k_value, num_cpus)  # Train our k-NN algorithm
    joblib.dump(classifier, knn_trained_classifier)  # Save the k-NN classifier so we can re-use it
    print("Saved the trained k-NN classifier to " + knn_trained_classifier + "\n")
